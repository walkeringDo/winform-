﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Z.EntityFramework.Plus;
using Zxw.Framework.NetCore.Extensions;
using Zxw.Framework.NetCore.Models;
using Zxw.Framework.NetCore.Options;
using DbType = Zxw.Framework.NetCore.Options.DbType;

namespace Zxw.Framework.NetCore.EfDbContext
{
    /// <summary>
    /// DbContext上下文的实现
    /// </summary>
    public sealed class DefaultDbContext : DbContext
    {
        private readonly DbContextOption _option;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public DefaultDbContext(IOptions<DbContextOption> option)
        {
            if(option==null)
                throw new ArgumentNullException(nameof(option));
            if(string.IsNullOrEmpty(option.Value.ConnectionString))
                throw new ArgumentNullException(nameof(option.Value.ConnectionString));
            if (string.IsNullOrEmpty(option.Value.ModelAssemblyName))
                throw new ArgumentNullException(nameof(option.Value.ModelAssemblyName));
            _option = option.Value;
        }
          /// <summary>
         /// 初始化一个 使用指定数据连接名称或连接串 的数据访问上下文类 的新实例
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (_option.DbType)
            {
                case DbType.ORACLE:
                    throw new NotSupportedException("Oracle for EF Core Database Provider is not yet available.");
                case DbType.MYSQL:
                    optionsBuilder.UseMySql(_option.ConnectionString);
                    break;
                case DbType.SQLITE:
                    optionsBuilder.UseSqlite(_option.ConnectionString);
                    break;
                case DbType.MEMORY:
                    optionsBuilder.UseInMemoryDatabase(_option.ConnectionString);
                    break;
                case DbType.NPGSQL:
                    optionsBuilder.UseNpgsql(_option.ConnectionString);
                    break;
                default:
                    optionsBuilder.UseSqlServer(_option.ConnectionString);
                    break;
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AddEntityTypes(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void AddEntityTypes(ModelBuilder modelBuilder)
        {
            var assembly = Assembly.Load(_option.ModelAssemblyName);
            var types = assembly?.GetTypes();
            var list = types?.Where(t =>
                t.IsClass && !t.IsGenericType && !t.IsAbstract &&
                t.GetInterfaces().Any(m => m.GetGenericTypeDefinition() == typeof(IBaseModel<>))).ToList();
            if (list != null && list.Any())
            {
                list.ForEach(t =>
                {
                    if (modelBuilder.Model.FindEntityType(t) == null)
                        modelBuilder.Model.AddEntityType(t);
                });
            }
        }

        /// <summary>
        /// ExecuteSqlWithNonQuery
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteSqlWithNonQuery(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(sql,
                CancellationToken.None,
                parameters);
        }

        /// <summary>
        /// edit an entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Edit<T>(T entity) where T : class
        {
            Entry(entity).State = EntityState.Modified;
            //return SaveChanges();
        }

        /// <summary>
        /// edit entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public void EditRange<T>(ICollection<T> entities) where T : class
        {
            Set<T>().AttachRange(entities.ToArray());
            //return SaveChanges();
        }

        /// <summary>
        /// update data by columns.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="updateColumns"></param>
        /// <returns></returns>
        public void Update<T>(T model, params string[] updateColumns) where T : class
        {
            if (updateColumns != null && updateColumns.Length > 0)
            {
                if (Entry(model).State == EntityState.Added ||
                    Entry(model).State == EntityState.Detached) Set<T>().Attach(model);
                foreach (var propertyName in updateColumns)
                {
                    Entry(model).Property(propertyName).IsModified = true;
                }
            }
            else
            {
                Entry(model).State = EntityState.Modified;
            }
            //return SaveChanges();
        }

        /// <summary>
        /// delete by query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public void Delete<T>(Expression<Func<T, bool>> @where) where T : class
        {
            Set<T>().Where(@where).Delete();
            //return SaveChanges();
        }

        /// <summary>
        /// bulk insert by sqlbulkcopy, and with transaction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="destinationTableName"></param>
        public void BulkInsert<T, TKey>(IList<T> entities, string destinationTableName = null) where T : class, IBaseModel<TKey>
        {
            if (entities == null || !entities.Any()) return;
            if (string.IsNullOrEmpty(destinationTableName))
            {
                var mappingTableName = typeof(T).GetCustomAttribute<TableAttribute>()?.Name;
                destinationTableName = string.IsNullOrEmpty(mappingTableName) ? typeof(T).Name : mappingTableName;
            }
            if (_option.DbType == DbType.MSSQLSERVER)
                SqlBulkInsert<T,TKey>(entities, destinationTableName);
            else if (_option.DbType == DbType.MYSQL)
                MySqlBulkInsert(entities, destinationTableName);
            else throw new NotSupportedException("This method only support for SQL Server or MySql.");

        }

        private void SqlBulkInsert<T,TKey>(IList<T> entities, string destinationTableName = null) where T : class,IBaseModel<TKey>
        {
            using (var dt = entities.ToDataTable())
            {
                dt.TableName = destinationTableName;
                using (var conn = Database.GetDbConnection() as SqlConnection ?? new SqlConnection(_option.ConnectionString))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    ;
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            var bulk = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran)
                            {
                                BatchSize = entities.Count,
                                DestinationTableName = dt.TableName,
                            };
                            GenerateColumnMappings<T, TKey>(bulk.ColumnMappings);
                            bulk.WriteToServer(dt);
                            tran.Commit();
                        }
                        catch (Exception e)
                        {
                            string message= e.Message;
                            tran.Rollback();
                            throw ;
                        }                        
                    }
                    conn.Close();
                }
            }
        }

        private void GenerateColumnMappings<T, TKey>(SqlBulkCopyColumnMappingCollection mappings)
            where T : class, IBaseModel<TKey>
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttributes<KeyAttribute>().Any())
                {
                    mappings.Add(new SqlBulkCopyColumnMapping(property.Name, typeof(T).Name + property.Name));
                }
                else
                {
                    mappings.Add(new SqlBulkCopyColumnMapping(property.Name, property.Name));                    
                }
            }
        }

        private void MySqlBulkInsert<T>(IList<T> entities, string destinationTableName) where T : class
        {
            var tmpDir = Path.Combine(AppContext.BaseDirectory, "Temp");
            if (!Directory.Exists(tmpDir))
                Directory.CreateDirectory(tmpDir);
            var csvFileName = Path.Combine(tmpDir, $"{DateTime.Now:yyyyMMddHHmmssfff}.csv");
            if (!File.Exists(csvFileName))
                File.Create(csvFileName);
            var separator = ",";
            entities.SaveToCsv(csvFileName, separator);
            using (var conn = Database.GetDbConnection() as MySqlConnection ?? new MySqlConnection(_option.ConnectionString))
            {
                conn.Open();
                var bulk = new MySqlBulkLoader(conn)
                {
                    NumberOfLinesToSkip = 0,
                    TableName = destinationTableName,
                    FieldTerminator = separator,
                    FieldQuotationCharacter = '"',
                    EscapeCharacter = '"',
                    LineTerminator = "\r\n"
                };
                bulk.Load();
                conn.Close();
            }
            File.Delete(csvFileName);
        }

        public List<TView> SqlQuery<T,TView>(string sql, params object[] parameters) 
            where T : class
            where TView : class
        {
            return Set<T>().FromSql(sql, parameters).Cast<TView>().ToList();
        }
    }
}
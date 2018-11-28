using DistributionWebApi.Models;
using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DistributionWebApi.App_Start
{
    public class RepositoryBase<TEntity, TIdentifier> where TEntity : EntityBase
    {
        protected IElasticClient _client;
        private static readonly string _scrollTime = "5m";
        //private string _connectionName;
        private object _lockObj = new object();

        public RepositoryBase()
        {
            CreateElasticSearchClient();
        }
        private void CreateElasticSearchClient()
        {
            lock (_lockObj)
            {
                if (_client == null)
                {
                    var connectionString = ConfigurationManager.AppSettings["ElasticUri"];
                    var uri = new Uri(connectionString, UriKind.Absolute);
                    _client = CreateClient(uri);
                }
            }
        }
        private static IElasticClient CreateClient(Uri connectionString)
        {
            var node = new UriBuilder(connectionString);

            var connectionPool = new SingleNodeConnectionPool(node.Uri);
            var connectionSettings = new ConnectionSettings(connectionPool);

            return new ElasticClient(connectionSettings);
        }

        public TEntity GetById(TIdentifier id)
        {
            var response = _client.Get<TEntity>(new DocumentPath<TEntity>(id.ToString()), g => g.Index(GetIndex<TEntity>()));
            if (response.Source != null)
                response.Source.Id = response.Id;
            return response.Source;
        }

        public List<TEntity> GetAll()
        {
            List<TEntity> all = null;
            var result = GetPaginatedData(0, 100, new QueryContainer { });
            //all = result.Results;
            all = result;
            return all;
        }

        public List<TEntity> GetAllRaw()
        {
            ISearchResponse<TEntity> response = _client.Search<TEntity>(s => s
              .Index(GetIndex<TEntity>())
              .From(0)
              .Size(50)
              .SearchType(Elasticsearch.Net.SearchType.QueryThenFetch));

            return response.Documents.ToList();
        }

        private List<TEntity> GetPaginatedData(int from, int size, QueryContainer query = null)
        {
            ISearchResponse<TEntity> response = _client.Search<TEntity>(s => s
               .Index(GetIndex<TEntity>())
               .From(from)
               .Size(size)
               .Query(q => query)
               .Source(sf => sf.Excludes(f => f.Fields("parameter")))
               .SearchType(Elasticsearch.Net.SearchType.QueryThenFetch));

            return GetSearchResults(response);
        }

        private static List<TEntity> GetSearchResults(ISearchResponse<TEntity> response)
        {
            var documents = response.Hits.Select(hit =>
            {
                var result = hit.Source;
                result.Id = hit.Id;
                return result;
            }).ToList();

            return new List<TEntity>(documents); //, response.ScrollId
        }

        public string Insert(TEntity doc)
        {
            var response = _client.Index<TEntity>(doc, g => g.Index(GetIndex<TEntity>()));
            return response.Id;
        }

        private static string GetIndex<T>()
        {
            var details = GetCustomAttribute<T, ElasticIndexDetailsAttribute>();
            if (details == null)
            {
                throw new Exception("ElasticIndexDetailsAttribute has not been set for the type " + typeof(T).FullName);
            }
            var index = details.IndexName;

            if (!details.IsTimeSeries)
            {
                return index;
            }
            var date = String.Format("{0:yyyy.MM.dd}", DateTime.Today);

            return $"{index}-{date}";
        }

        private static string GetSettingByKey(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? string.Empty;
        }

        private static TAttr GetCustomAttribute<T, TAttr>()
        {
            Type type = typeof(T);
            TAttr[] attribs = type.GetCustomAttributes(
                 typeof(TAttr), false) as TAttr[];
            return attribs[0];
        }
    }
}
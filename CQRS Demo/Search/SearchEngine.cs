using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CQRS_Demo.Search
{
    public class SearchEngine
    {
        protected FSDirectory _directoryTemp;
        protected string _indexFolder;
        protected Lucene.Net.Util.Version _luceneVersion = Lucene.Net.Util.Version.LUCENE_30;

        public QueryParser BuildParser(List<QueryTerm> queryTerms, StandardAnalyzer analyzer)
        {
            if (queryTerms.Count > 1)
            {
                var fields = queryTerms.Select(x => x.Field).ToArray();
                var multiFieldParser = new MultiFieldQueryParser(_luceneVersion, fields, analyzer);

                return multiFieldParser;
            }
            else
            {
                var queryTerm = queryTerms.First();
                var standardParser = new QueryParser(_luceneVersion, queryTerm.Field, analyzer)
                {
                    DefaultOperator = QueryParser.Operator.AND
                };

                return standardParser;
            }
        }



        protected Query ParseQuery(List<QueryTerm> queryTerms, QueryParser parser)
        {
            Query query;
            parser.AllowLeadingWildcard = true;

            if (queryTerms.Count > 1)
            {
                query = CreateQueryForMultipleTerms(queryTerms);
            }
            else
            {
                var queryTerm = queryTerms.First();

                if (queryTerm.IsCombinationTerms())
                {
                    query = CreateSingleTermCombinationQuery(queryTerm);
                }
                else
                {
                    query = CreateSingleTermQuery(parser, queryTerm);
                }
            }

            return query;
        }

        protected Query CreateSingleTermQuery(QueryParser parser, QueryTerm queryTerm)
        {
            var singleValueQuery = parser.Parse($"*{queryTerm.Term}*");

            return singleValueQuery;
        }

        protected Query CreateSingleTermCombinationQuery(QueryTerm queryTerm)
        {
            var booleanQuery = new BooleanQuery();
            foreach (var term in queryTerm.Terms())
            {
                var luceneTerm = new Term(queryTerm.Field, $"*{term.ToLower()}*");
                var fieldQuery = new WildcardQuery(luceneTerm);
                booleanQuery.Add(fieldQuery, Occur.SHOULD);
            }

            return booleanQuery;
        }

        protected Query CreateQueryForMultipleTerms(List<QueryTerm> queryTerms)
        {
            var booleanQuery = new BooleanQuery();
            foreach (var queryTerm in queryTerms)
            {
                var innerQuery = new BooleanQuery();

                foreach (var term in queryTerm.Terms())
                {
                    var luceneTerm = new Term(queryTerm.Field, $"*{term.ToLower()}*");
                    var fieldQuery = new WildcardQuery(luceneTerm);
                    innerQuery.Add(fieldQuery, Occur.SHOULD);
                }

                booleanQuery.Add(innerQuery, Occur.SHOULD);
            }

            return booleanQuery;
        }

        protected FSDirectory Directory
        {
            get
            {
                if (_directoryTemp == null)
                    _directoryTemp = FSDirectory.Open(new DirectoryInfo(IndexFolder));

                if (IndexWriter.IsLocked(_directoryTemp))
                    IndexWriter.Unlock(_directoryTemp);

                var lockFilePath = Path.Combine(IndexFolder, "write.lock");

                if (File.Exists(lockFilePath))
                {
                    try
                    {
                        File.Delete(lockFilePath);
                    }
                    catch (Exception)
                    {//TODO: figout out logging here

                    }
                }


                return _directoryTemp;
            }
        }

        public string IndexFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_indexFolder))
                {
                    _indexFolder = Path.GetTempPath();
                }

                return _indexFolder;
            }
            set { _indexFolder = value; }
        }

        public int MaxHitsAllowed { get; set; } = 1000;

        public static void Start()
        {

        }

        public static void Stop()
        {
        }
    }
}

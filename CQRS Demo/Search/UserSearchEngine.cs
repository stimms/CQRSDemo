using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRS_Demo.Search
{
    public class UserSearchEngine:SearchEngine
    {
        public void ReBuildIndex(List<UserSearchItem> records)
        {
            ClearCache();
            BuildIndex(records);
        }
        public void BuildIndex(List<UserSearchItem> records)
        {
            using (var analyzer = new StandardAnalyzer(_luceneVersion))
            {
                using (var writer = new IndexWriter(Directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    foreach (var record in records)
                    {
                        writer.AddDocument(MapToDocument(record));
                    }
                }
            }
        }
        public Document MapToDocument(UserSearchItem record, Document document = null)
        {
            if (document == null)
                document = new Document();

            document.Add(new Field(UserSearchFields.UserId, record.UserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            document.Add(new Field(UserSearchFields.FirstName, record.FirstName.ToLower(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            document.Add(new Field(UserSearchFields.LastName, record.LastName.ToLower(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            return document;
        }

        public void Update(UserSearchItem record)
        {
            var document = MapToDocument(record);
            using (var analyzer = new StandardAnalyzer(_luceneVersion))
            {
                using (var writer = new IndexWriter(Directory, analyzer, false, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.UpdateDocument(new Term(UserSearchFields.UserId, record.UserId.ToString()), document);
                }
            }
        }

        public List<UserSearchItem> Query(List<QueryTerm> queryTerms)
        {
            var queryResults = new List<UserSearchItem>();
            using (var searcher = new IndexSearcher(Directory, false))
            {
                using (var analyzer = new StandardAnalyzer(_luceneVersion))
                {
                    var parser = BuildParser(queryTerms, analyzer);
                    var boolQuery = ParseQuery(queryTerms, parser);
                    var searchResults = searcher.Search(boolQuery, null, MaxHitsAllowed);
                    var hits = searchResults.ScoreDocs.Where(x => x.Score == searchResults.MaxScore).ToList();

                    queryResults = hits.Select(x => MapHitToQueryResult(x, searcher)).ToList();
                }
            }
            return queryResults;
        }

        private UserSearchItem MapHitToQueryResult(ScoreDoc doc, IndexSearcher searcher)
        {
            var document = searcher.Doc(doc.Doc);

            var result = new UserSearchItem
            {
                UserId = Int32.Parse(document.Get(UserSearchFields.UserId)),
                FirstName = document.Get(UserSearchFields.FirstName),
                LastName = document.Get(UserSearchFields.LastName)
            };
            
            return result;
        }

        private void ClearCache()
        {
            using (var analyzer = new StandardAnalyzer(_luceneVersion))
            {
                using (var writer = new IndexWriter(Directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entries
                    writer.DeleteAll();

                }
            }

        }
    }
    public class UserSearchItem
    {

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UserSearchFields
    {
        public static string FirstName = "FirstName";
        public static string LastName = "LastName";
        public static string UserId = "UserId";
    }
}
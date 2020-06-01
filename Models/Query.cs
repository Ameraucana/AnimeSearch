using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AnimeSearch.Models
{
    class Query
    {
        public Query(string query, Dictionary<string, object> variables)
        {
            this.query = query;
            this.variables = variables;
        }
        public string query;
        public Dictionary<string, object> variables;

    }
}

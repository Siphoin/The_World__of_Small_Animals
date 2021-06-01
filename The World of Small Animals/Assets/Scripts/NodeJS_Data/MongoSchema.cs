using Newtonsoft.Json;
using System;
[Serializable]
  public  class MongoSchema
    {
    [JsonProperty("_id")]
    public string id;

    [JsonProperty("__v")]
    public long versionDocument = 0;
}
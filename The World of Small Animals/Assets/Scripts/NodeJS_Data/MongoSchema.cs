using Newtonsoft.Json;
using System;

[Serializable]
  public  class MongoSchema
    {
    private string _id;

    private long _versionDocument = 0;

    [JsonProperty("_id")]
    public string Id => _id;

    [JsonProperty("__v")]
    public long VersionDocument => _versionDocument;
}
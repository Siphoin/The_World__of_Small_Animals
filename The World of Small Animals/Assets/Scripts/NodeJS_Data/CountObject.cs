[System.Serializable]
 public   struct CountObject
    {
    private long _count;

    public long Count => _count;

    public CountObject (long count)
    {
        _count = count;
    }

}

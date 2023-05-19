namespace SWI_Simulation.DataType
{
    public static class RegexPattern
    {
        public const string FACT_PATTERN = @"([^,]+\(.+?\))|([^,]+)";
        public const string COMPOUND_TERN_PATTERN = @"([^,]+\(.+?\))";
        public const string COMPARISION_OPERATION_PATTERN = @"(\\\=)|(\=)|(\>)|(\<)|(\<\=)|(\>\=)";
        public const string BINARY_CALC_PATTERN = @"(\\\=)|(\=)|(\\)|(\+)";
        public const string COMMENT_PATTERN = @"(\%.+)|(\/\*(.+)|(\n+.+)\*\/)";
        public const string MULTI_NEWLINE = @"\n+";
        public const string QUERY_SIMPLE_PARTTERN = @"\?-\s+\w+\(.+\)";
        public const string COMPARISION_ARGS_PATTERN = @"([^\\\=\s\']+)|('.+')";
    }
    public enum TernType
    {
        Comparision,
        Atom,
        Number,
        Variable,
        CompoundTerm
    }
}
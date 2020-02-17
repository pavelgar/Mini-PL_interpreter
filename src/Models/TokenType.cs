namespace miniPL {
    public enum TokenType {
        // Keywords
        VAR,
        FOR,
        END,
        IN,
        DO,
        READ,
        PRINT,
        INT,
        STR,
        BOOL,
        ASSERT,

        // Arithmetic
        ADD,
        SUB,
        MULT,
        DIV,

        // Logic
        LT,
        EQ,
        AND,
        NOT,

        // Literals
        IDENT,
        INTEGER,
        STRING,

        // Other
        COLON,
        SEMICOLON,
        RANGE,
        LEFT_PAREN,
        RIGHT_PAREN,
        ASSIGN,

        EOF,
        SCAN_ERROR
    }
}

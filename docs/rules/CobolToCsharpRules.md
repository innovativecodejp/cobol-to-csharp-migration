# Rule Definitions
COBOL to C# Migration Framework

Last Updated: 2026-02-18
Version: MVP14

---

## Overview

This document defines the transformation rules for converting COBOL constructs to C# equivalents. Each rule is designed to preserve business logic while adapting to modern C# patterns and practices.

---

## Rule Categories

### R-001 Series: Data Movement Operations

#### R-001-01: MOVE Statement
**COBOL Pattern:**
```cobol
MOVE source-field TO target-field
```

**C# Transformation:**
```csharp
targetField = sourceField;
```

**Implementation Status:** ✅ Implemented
**Test Coverage:** Complete

---

### R-002 Series: String Operations

#### R-002-01: UNSTRING Statement
**COBOL Pattern:**
```cobol
UNSTRING input-string 
    DELIMITED BY delimiter
    INTO field1 field2 ...
```

**C# Transformation:**
```csharp
string[] parts = inputString.Trim().Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
field1 = parts.Length > 0 ? parts[0] : string.Empty;
field2 = parts.Length > 1 ? parts[1] : string.Empty;
// ... additional fields as needed
```

**Implementation Status:** ✅ Implemented
**Test Coverage:** Basic scenarios covered
**Known Limitations:** 
- Multiple delimiter patterns not fully supported
- Complex INTO clause variations pending

**MVP03 Verified Derived Features:**
- `DELIMITED BY ALL SPACE`
- `COUNT IN` per receiving field
- `WITH POINTER` (1-based pointer behavior)
- `TALLYING IN` (receiving-field count behavior)

**MVP03 Verification Sample:**
```cobol
UNSTRING WS-IN
    DELIMITED BY ALL SPACE
    INTO WS-A COUNT IN WS-LEN-A
         WS-B COUNT IN WS-LEN-B
         WS-C COUNT IN WS-LEN-C
    WITH POINTER WS-PTR
    TALLYING IN WS-DELIM-COUNT
END-UNSTRING
```

**MVP03 Test Scope:**
- Consecutive space variations (`ALL SPACE` behavior)
- Trailing spaces
- Leading spaces
- No-delimiter input (fixed `PTR` / `DC` behavior)

#### R-002-02: INSPECT Statement
**COBOL Pattern:**
```cobol
INSPECT field-name CONVERTING source-chars TO target-chars
```

**C# Transformation:**
```csharp
fieldName = fieldName.ToUpperInvariant(); // for lowercase to uppercase conversion
// Additional character conversion logic as needed
```

**Implementation Status:** ✅ Implemented (MVP04/MVP05 scope)
**Test Coverage:** Extended scenarios covered

**MVP04 Verified Derived Features:**
- `TALLYING ... FOR ALL "A"`
- `REPLACING LEADING "0" BY "X"`
- `REPLACING FIRST "AB" BY "YZ"`

**MVP05 Verified Derived Features:**
- `CONVERTING "ABCDEFGHIJKLMNOPQRSTUVWXYZ" TO "abcdefghijklmnopqrstuvwxyz"`

**MVP04/MVP05 Verification Samples:**
```cobol
INSPECT WS-TEXT TALLYING WS-COUNT FOR ALL "A"
INSPECT WS-TEXT REPLACING LEADING "0" BY "X"
INSPECT WS-TEXT REPLACING FIRST "AB" BY "YZ"
INSPECT WS-TEXT CONVERTING "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                     TO "abcdefghijklmnopqrstuvwxyz"
```

**Known Limitations:**
- Character-class and locale-specific conversions are not covered yet
- Multi-byte and national character handling is out of current scope

---

### R-005 Series: Control Flow Operations

#### R-005-01: EVALUATE Statement
**COBOL Pattern:**
```cobol
EVALUATE WS-MODE
    WHEN 1
        EVALUATE WS-VAL
            WHEN 0 THRU 9
            WHEN 10 THRU 19
            WHEN OTHER
        END-EVALUATE
    WHEN OTHER
        EVALUATE TRUE ALSO TRUE
            WHEN (cond-1) ALSO (cond-2)
            WHEN OTHER
        END-EVALUATE
END-EVALUATE
```

**C# Transformation:**
```csharp
if (wsMode == 1)
{
    if (wsVal >= 0 && wsVal <= 9) { wsRange = "0-9"; }
    else if (wsVal >= 10 && wsVal <= 19) { wsRange = "10-19"; }
    else { wsRange = "OTHER"; }
}
else
{
    if ((wsMode == 2) && (wsVal >= 200)) { wsCase = "VIP"; }
    else if ((wsMode == 2) && (wsVal < 200)) { wsCase = "NORMAL"; }
    else { wsCase = "N/A"; }
}
```

**Implementation Status:** ✅ Implemented (MVP06 minimal scope)
**Test Coverage:** THRU + ALSO branches covered
**MVP07/MVP08 Verified Derived Features:**
- Multiple `WHEN` branches
- `THRU` range matching (`0 THRU 9`, `10 THRU 19`)
- `ALSO` condition pairs (`EVALUATE TRUE ALSO TRUE`)

---

#### R-005-02: PERFORM Paragraph / THRU
**COBOL Pattern:**
```cobol
IF WS-FLAG = 1
    PERFORM PARA-MARK
END-IF

PERFORM PARA-A THRU PARA-C-EXIT
```

**C# Transformation:**
```csharp
if (wsFlag == 1) { ParaMark(ref wsMark); }
ParaA(wsOut);
ParaB(wsOut);
ParaC(wsOut);
ParaCExit();
```

**Implementation Status:** ✅ Implemented (MVP09 minimal scope)
**Test Coverage:** Paragraph call + THRU range execution covered
**MVP09 Verified Derived Features:**
- Single paragraph `PERFORM`
- `PERFORM ... THRU` contiguous paragraph execution
- Exit-paragraph boundary handling

---

#### R-005-03: PERFORM UNTIL
**COBOL Pattern:**
```cobol
MOVE 1 TO WS-I
MOVE 0 TO WS-SUM
PERFORM UNTIL WS-I > WS-N
    ADD WS-I TO WS-SUM
    ADD 1 TO WS-I
END-PERFORM
```

**C# Transformation:**
```csharp
int wsI = 1;
int wsSum = 0;
while (wsI <= wsN)
{
    wsSum += wsI;
    wsI += 1;
}
```

**Implementation Status:** ✅ Implemented (MVP10 minimal scope)
**Test Coverage:** Normal path + zero-iteration path covered
**MVP10 Verified Derived Features:**
- Test-before loop behavior
- Immediate termination case (`N=0`)

---

#### R-005-04: PERFORM VARYING (FROM/BY/UNTIL)
**COBOL Pattern:**
```cobol
MOVE 0 TO WS-SUM
PERFORM VARYING WS-I FROM 1 BY 2 UNTIL WS-I > WS-N
    ADD WS-I TO WS-SUM
END-PERFORM
```

**C# Transformation:**
```csharp
int wsSum = 0;
for (int wsI = 1; wsI <= wsN; wsI += 2)
{
    wsSum += wsI;
}
```

**Implementation Status:** ✅ Implemented (MVP11 minimal scope)
**Test Coverage:** Odd-step accumulation + zero-iteration path covered
**MVP11 Verified Derived Features:**
- `FROM` initialization
- `BY` step increment
- `UNTIL` termination condition

---

### R-003 Series: I/O Operations

#### R-003-01: DISPLAY Statement
**COBOL Pattern:**
```cobol
DISPLAY field-name
```

**C# Transformation:**
```csharp
Console.WriteLine(fieldName);
```

**Implementation Status:** ✅ Implemented
**Test Coverage:** Complete

---

### R-004 Series: File I/O Operations

#### R-004-01: Sequential READ with `AT END`
**COBOL Pattern:**
```cobol
OPEN INPUT IN-FILE
PERFORM UNTIL EOF
    READ IN-FILE
        AT END
            SET EOF TO TRUE
        NOT AT END
            ADD 1 TO WS-LINE-NO
            DISPLAY "LINE=" WS-LINE-NO "|TEXT=" IN-REC
    END-READ
END-PERFORM
CLOSE IN-FILE
DISPLAY "COUNT=" WS-LINE-NO
```

**C# Transformation:**
```csharp
int lineNo = 0;
bool eof = false;
using (var reader = new StreamReader(inputPath, Encoding.ASCII))
{
    while (!eof)
    {
        string record = reader.ReadLine();
        if (record == null) { eof = true; }
        else
        {
            lineNo += 1;
            writer.WriteLine("LINE=" + lineNo.ToString("D4") + "|TEXT=" + record);
        }
    }
}
writer.WriteLine("COUNT=" + lineNo.ToString("D4"));
```

**Implementation Status:** ✅ Implemented (MVP12 minimal scope)
**Test Coverage:** Multi-line input + EOF termination path covered
**MVP12 Verified Features:**
- `SELECT/FD` with line-sequential input
- `READ ... AT END` EOF control
- Runtime path resolution (`MVP12_INPUT` or `mvp12-input.txt`)

---

#### R-004-02: Sequential WRITE (output file creation)
**COBOL Pattern:**
```cobol
OPEN OUTPUT OUT-FILE
PERFORM 3 TIMES
    ADD 1 TO WS-I
    STRING "REC" DELIMITED BY SIZE
           "="   DELIMITED BY SIZE
           WS-I  DELIMITED BY SIZE
           INTO OUT-REC
    END-STRING
    WRITE OUT-REC
END-PERFORM
CLOSE OUT-FILE
DISPLAY "WROTE=0003"
```

**C# Transformation:**
```csharp
using (var fileWriter = new StreamWriter(outputPath, false, Encoding.ASCII))
{
    for (int i = 1; i <= 3; i++)
    {
        fileWriter.WriteLine("REC=" + i.ToString("D4"));
    }
}
writer.WriteLine("WROTE=0003");
```

**Implementation Status:** ✅ Implemented (MVP13 minimal scope)
**Test Coverage:** Generated output-file content exact match covered
**MVP13 Verified Features:**
- `SELECT/FD` with line-sequential output
- Basic `WRITE` loop (`PERFORM n TIMES`)
- Runtime path resolution (`MVP13_OUTPUT` or `mvp13-output.txt`)

---

#### R-004-03: Indexed START positioning + `READ NEXT`
**COBOL Pattern:**
```cobol
START IDX-FILE KEY >= WS-STARTKEY
    INVALID KEY
        SET EOF TO TRUE
END-START

PERFORM UNTIL EOF
    READ IDX-FILE NEXT
        AT END
            SET EOF TO TRUE
        NOT AT END
            DISPLAY "KEY=" IDX-KEY "|TEXT=" IDX-TEXT
    END-READ
END-PERFORM
DISPLAY "DONE"
```

**C# Transformation:**
```csharp
int startIndex = records.FindIndex(r => r.Key >= startKey);
if (startIndex >= 0)
{
    for (int i = startIndex; i < records.Count; i++)
    {
        writer.WriteLine("KEY=" + records[i].Key.ToString("D4") + "|TEXT=" + records[i].Text);
    }
}
writer.WriteLine("DONE");
```

**Implementation Status:** ✅ Implemented (MVP14 minimal scope)
**Test Coverage:** Case-based start-key scenarios covered
**MVP14 Verified Features:**
- `ORGANIZATION IS INDEXED` + `RECORD KEY`
- `START ... KEY >= ...` positioning semantics
- `READ ... NEXT` sequential fetch after positioning
- Runtime controls (`MVP14_INDEX`, `MVP14_CASE`)

---

## Rule Implementation Guidelines

### Transformation Principles

1. **Business Logic Preservation**: All transformations must preserve the original business logic intent
2. **Type Safety**: Leverage C# type system for improved reliability
3. **Performance Consideration**: Optimize for modern runtime characteristics
4. **Maintainability**: Generate readable, maintainable C# code

### Error Handling Strategy

- Use exceptions for unrecoverable errors
- Implement validation for data type conversions
- Preserve COBOL error handling semantics where applicable

### Testing Requirements

Each rule must include:
- Unit tests for typical scenarios
- Edge case validation
- Error condition handling
- Performance benchmarks (where applicable)

---

## Rule Versioning

Rules follow semantic versioning:
- Major version: Breaking changes to transformation logic
- Minor version: New rule additions or enhancements
- Patch version: Bug fixes and clarifications

---

## Extension Points

### Custom Rule Integration

The framework supports custom rule definitions for:
- Organization-specific COBOL patterns
- Legacy system peculiarities
- Domain-specific transformations

### Rule Validation

All rules undergo:
- Automated testing
- Manual review
- Production validation (where possible)

---

## Future Rule Categories

Planned rule categories for future releases:

- **R-005 Series**: Control Flow extensions (IF, GO TO, nested control variants)
- **R-006 Series**: Arithmetic Operations
- **R-007 Series**: Data Definition (PIC clauses, OCCURS)
- **R-008 Series**: Program Structure (DIVISION, SECTION)

---

Status: Active Development
Stability: MVP Level
Next Review: After MVP14 stabilization
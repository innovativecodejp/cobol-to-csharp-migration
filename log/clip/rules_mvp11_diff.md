docs/rules/CobolToCsharpRules.md (diff excerpt)
Rule Update Summary

diff --git a/docs/rules/CobolToCsharpRules.md b/docs/rules/CobolToCsharpRules.md
index 86a968a..d24f2d8 100644
--- a/docs/rules/CobolToCsharpRules.md
+++ b/docs/rules/CobolToCsharpRules.md
@@ -1,8 +1,8 @@
 # Rule Definitions
 COBOL to C# Migration Framework

-Last Updated: 2026-02-17
-Version: MVP06
+Last Updated: 2026-02-18
+Version: MVP11

 ---

@@ -156,13 +156,98 @@ else

 **Implementation Status:** 笨・Implemented (MVP06 minimal scope)
 **Test Coverage:** THRU + ALSO branches covered
-**MVP06 Verified Derived Features:**
+**MVP07/MVP08 Verified Derived Features:**
 - Multiple `WHEN` branches
 - `THRU` range matching (`0 THRU 9`, `10 THRU 19`)
 - `ALSO` condition pairs (`EVALUATE TRUE ALSO TRUE`)

 ---

+#### R-005-02: PERFORM Paragraph / THRU
+**COBOL Pattern:**
+```cobol
+IF WS-FLAG = 1
+    PERFORM PARA-MARK
+END-IF
+
+PERFORM PARA-A THRU PARA-C-EXIT
+```
+
+**C# Transformation:**
+```csharp
+if (wsFlag == 1) { ParaMark(ref wsMark); }
+ParaA(wsOut);
+ParaB(wsOut);
+ParaC(wsOut);
+ParaCExit();
+```
+
+**Implementation Status:** 笨・Implemented (MVP09 minimal scope)
+**Test Coverage:** Paragraph call + THRU range execution covered
+**MVP09 Verified Derived Features:**
+- Single paragraph `PERFORM`
+- `PERFORM ... THRU` contiguous paragraph execution
+- Exit-paragraph boundary handling
+
+---
+
+#### R-005-03: PERFORM UNTIL
+**COBOL Pattern:**
+```cobol
+MOVE 1 TO WS-I
+MOVE 0 TO WS-SUM
+PERFORM UNTIL WS-I > WS-N
+    ADD WS-I TO WS-SUM
+    ADD 1 TO WS-I
+END-PERFORM
+```
+
+**C# Transformation:**
+```csharp
+int wsI = 1;
+int wsSum = 0;
+while (wsI <= wsN)
+{
+    wsSum += wsI;
+    wsI += 1;
+}
+```
+
+**Implementation Status:** 笨・Implemented (MVP10 minimal scope)
+**Test Coverage:** Normal path + zero-iteration path covered
+**MVP10 Verified Derived Features:**
+- Test-before loop behavior
+- Immediate termination case (`N=0`)
+
+---
+
+#### R-005-04: PERFORM VARYING (FROM/BY/UNTIL)
+**COBOL Pattern:**
+```cobol
+MOVE 0 TO WS-SUM
+PERFORM VARYING WS-I FROM 1 BY 2 UNTIL WS-I > WS-N
+    ADD WS-I TO WS-SUM
+END-PERFORM
+```
+
+**C# Transformation:**
+```csharp
+int wsSum = 0;
+for (int wsI = 1; wsI <= wsN; wsI += 2)
+{
+    wsSum += wsI;
+}
+```
+
+**Implementation Status:** 笨・Implemented (MVP11 minimal scope)
+**Test Coverage:** Odd-step accumulation + zero-iteration path covered
+**MVP11 Verified Derived Features:**
+- `FROM` initialization
+- `BY` step increment
+- `UNTIL` termination condition
+
+---
+
 ### R-003 Series: I/O Operations

 #### R-003-01: DISPLAY Statement
@@ -238,7 +323,7 @@ All rules undergo:
 Planned rule categories for future releases:

 - **R-004 Series**: File Operations (READ, WRITE, OPEN, CLOSE)
-- **R-005 Series**: Control Flow (IF, PERFORM, GO TO)
+- **R-005 Series**: Control Flow extensions (IF, GO TO, nested control variants)
 - **R-006 Series**: Arithmetic Operations
 - **R-007 Series**: Data Definition (PIC clauses, OCCURS)
 - **R-008 Series**: Program Structure (DIVISION, SECTION)
@@ -247,4 +332,4 @@ Planned rule categories for future releases:

 Status: Active Development
 Stability: MVP Level
-Next Review: After MVP06 stabilization
\ No newline at end of file
+Next Review: After MVP11 stabilization
\ No newline at end of file

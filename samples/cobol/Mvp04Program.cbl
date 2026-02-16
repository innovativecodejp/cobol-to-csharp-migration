       IDENTIFICATION DIVISION.
       PROGRAM-ID. MVP04PROGRAM.

       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01  WS-TEXT              PIC X(40).
       01  WS-COUNT             PIC 9(4) VALUE 0.

       PROCEDURE DIVISION.
           *> Read one input line into WS-TEXT.
           ACCEPT WS-TEXT

           *> If the input line starts with A, run TALLYING case.
           *> Otherwise run REPLACING case.
           IF WS-TEXT(1:1) = "A"
               INSPECT WS-TEXT TALLYING WS-COUNT FOR ALL "A"
               DISPLAY "TEXT=" WS-TEXT "|COUNT=" WS-COUNT
           ELSE
               INSPECT WS-TEXT REPLACING LEADING "0" BY "X"
               INSPECT WS-TEXT REPLACING FIRST "AB" BY "YZ"
               DISPLAY "TEXT=" WS-TEXT
           END-IF

           GOBACK.

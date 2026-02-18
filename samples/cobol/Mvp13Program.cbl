       IDENTIFICATION DIVISION.
       PROGRAM-ID. MVP13PROGRAM.

       ENVIRONMENT DIVISION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
           SELECT OUT-FILE ASSIGN TO DYNAMIC WS-OUT-PATH
               ORGANIZATION IS LINE SEQUENTIAL.

       DATA DIVISION.
       FILE SECTION.
       FD  OUT-FILE.
       01  OUT-REC               PIC X(80).

       WORKING-STORAGE SECTION.
       01  WS-OUT-PATH           PIC X(260).
       01  WS-I                  PIC 9(4) VALUE 0.
       01  WS-TEXT               PIC X(10) VALUE SPACES.

       PROCEDURE DIVISION.
           MOVE "mvp13-output.txt" TO WS-OUT-PATH.
           ACCEPT WS-OUT-PATH FROM ENVIRONMENT "MVP13_OUTPUT".
           IF WS-OUT-PATH = SPACES
               MOVE "mvp13-output.txt" TO WS-OUT-PATH
           END-IF.

           OPEN OUTPUT OUT-FILE.
           PERFORM 3 TIMES
               ADD 1 TO WS-I
               MOVE SPACES TO WS-TEXT
               MOVE "REC" TO WS-TEXT(1:3)
               MOVE SPACES TO OUT-REC
               STRING WS-TEXT(1:3) DELIMITED BY SIZE
                      "="          DELIMITED BY SIZE
                      WS-I         DELIMITED BY SIZE
                      INTO OUT-REC
               END-STRING
               WRITE OUT-REC
           END-PERFORM.
           CLOSE OUT-FILE.

           DISPLAY "WROTE=0003".
           GOBACK.

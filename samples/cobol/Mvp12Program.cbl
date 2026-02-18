       IDENTIFICATION DIVISION.
       PROGRAM-ID. MVP12PROGRAM.

       ENVIRONMENT DIVISION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
           SELECT IN-FILE ASSIGN TO DYNAMIC WS-IN-PATH
               ORGANIZATION IS LINE SEQUENTIAL.

       DATA DIVISION.
       FILE SECTION.
       FD  IN-FILE.
       01  IN-REC                PIC X(80).

       WORKING-STORAGE SECTION.
       01  WS-IN-PATH            PIC X(260).
       01  WS-LINE-NO            PIC 9(4) VALUE 0.
       01  WS-EOF                PIC X VALUE "N".
           88 EOF                VALUE "Y".
           88 NOT-EOF            VALUE "N".

       PROCEDURE DIVISION.
           MOVE "mvp12-input.txt" TO WS-IN-PATH.
           ACCEPT WS-IN-PATH FROM ENVIRONMENT "MVP12_INPUT".
           IF WS-IN-PATH = SPACES
               MOVE "mvp12-input.txt" TO WS-IN-PATH
           END-IF.

           OPEN INPUT IN-FILE.
           PERFORM UNTIL EOF
               READ IN-FILE
                   AT END
                       SET EOF TO TRUE
                   NOT AT END
                       ADD 1 TO WS-LINE-NO
                       DISPLAY "LINE=" WS-LINE-NO "|TEXT=" IN-REC
               END-READ
           END-PERFORM.
           CLOSE IN-FILE.

           DISPLAY "COUNT=" WS-LINE-NO.
           GOBACK.

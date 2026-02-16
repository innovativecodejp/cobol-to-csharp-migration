       IDENTIFICATION DIVISION.
       PROGRAM-ID. MVP03PROGRAM.

       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01  WS-IN                PIC X(15).
       01  WS-A                 PIC X(10).
       01  WS-B                 PIC X(10).
       01  WS-C                 PIC X(10).
       01  WS-PTR               PIC 9(4) VALUE 1.
       01  WS-DELIM-COUNT       PIC 9(4) VALUE 0.
       01  WS-LEN-A             PIC 9(4) VALUE 0.
       01  WS-LEN-B             PIC 9(4) VALUE 0.
       01  WS-LEN-C             PIC 9(4) VALUE 0.

       PROCEDURE DIVISION.
           MOVE "AAA   BBB  CCCC" TO WS-IN

           UNSTRING WS-IN
               DELIMITED BY ALL SPACE
               INTO WS-A COUNT IN WS-LEN-A
                    WS-B COUNT IN WS-LEN-B
                    WS-C COUNT IN WS-LEN-C
               WITH POINTER WS-PTR
               TALLYING IN WS-DELIM-COUNT
           END-UNSTRING

           DISPLAY "A=" WS-A
                   "|LA=" WS-LEN-A
                   "|B=" WS-B
                   "|LB=" WS-LEN-B
                   "|C=" WS-C
                   "|LC=" WS-LEN-C
                   "|PTR=" WS-PTR
                   "|DC=" WS-DELIM-COUNT

           GOBACK.

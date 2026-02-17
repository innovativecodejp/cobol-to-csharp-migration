       IDENTIFICATION DIVISION.
       PROGRAM-ID. MVP07PROGRAM.

       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01  WS-VAL               PIC 9(3).
       01  WS-RANGE             PIC X(10).

       PROCEDURE DIVISION.
           ACCEPT WS-VAL

           EVALUATE WS-VAL
               WHEN 0 THRU 9
                   MOVE "0-9" TO WS-RANGE
               WHEN 10 THRU 19
                   MOVE "10-19" TO WS-RANGE
               WHEN OTHER
                   MOVE "OTHER" TO WS-RANGE
           END-EVALUATE

           DISPLAY "VAL=" WS-VAL "|RANGE=" WS-RANGE

           GOBACK.

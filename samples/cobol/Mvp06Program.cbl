       IDENTIFICATION DIVISION.
       PROGRAM-ID. MVP06PROGRAM.

       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01  WS-LINE              PIC X(40).
       01  WS-MODE              PIC 9(2).
       01  WS-VAL               PIC 9(4).
       01  WS-RANGE             PIC X(10).
       01  WS-CASE              PIC X(10).

       PROCEDURE DIVISION.
           ACCEPT WS-LINE
           MOVE FUNCTION NUMVAL(WS-LINE(1:1)) TO WS-MODE
           MOVE FUNCTION NUMVAL(WS-LINE(3:38)) TO WS-VAL

           EVALUATE WS-MODE
               WHEN 1
                   EVALUATE WS-VAL
                       WHEN 0 THRU 9
                           MOVE "0-9" TO WS-RANGE
                       WHEN 10 THRU 19
                           MOVE "10-19" TO WS-RANGE
                       WHEN OTHER
                           MOVE "OTHER" TO WS-RANGE
                   END-EVALUATE
                   DISPLAY "MODE=" WS-MODE "|VAL=" WS-VAL "|RANGE=" WS-RANGE
               WHEN OTHER
                   EVALUATE TRUE ALSO TRUE
                       WHEN (WS-MODE = 2) ALSO (WS-VAL >= 200)
                           MOVE "VIP" TO WS-CASE
                       WHEN (WS-MODE = 2) ALSO (WS-VAL < 200)
                           MOVE "NORMAL" TO WS-CASE
                       WHEN OTHER
                           MOVE "N/A" TO WS-CASE
                   END-EVALUATE
                   DISPLAY "MODE=" WS-MODE "|VAL=" WS-VAL "|CASE=" WS-CASE
           END-EVALUATE

           GOBACK.

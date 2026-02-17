       IDENTIFICATION DIVISION.
       PROGRAM-ID. MVP10PROGRAM.

       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01  WS-N                 PIC 9(3) VALUE 0.
       01  WS-I                 PIC 9(3) VALUE 0.
       01  WS-SUM               PIC 9(5) VALUE 0.

       PROCEDURE DIVISION.
           ACCEPT WS-N

           MOVE 1 TO WS-I
           MOVE 0 TO WS-SUM

           PERFORM UNTIL WS-I > WS-N
               ADD WS-I TO WS-SUM
               ADD 1 TO WS-I
           END-PERFORM

           DISPLAY "N=" WS-N "|SUM=" WS-SUM

           GOBACK.

       IDENTIFICATION DIVISION.
       PROGRAM-ID. MVP11PROGRAM.

       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01  WS-N                 PIC 9(3) VALUE 0.
       01  WS-I                 PIC 9(3) VALUE 0.
       01  WS-SUM               PIC 9(5) VALUE 0.

       PROCEDURE DIVISION.
           ACCEPT WS-N

           MOVE 0 TO WS-SUM
           PERFORM VARYING WS-I FROM 1 BY 2 UNTIL WS-I > WS-N
               ADD WS-I TO WS-SUM
           END-PERFORM

           DISPLAY "N=" WS-N "|SUM=" WS-SUM

           GOBACK.

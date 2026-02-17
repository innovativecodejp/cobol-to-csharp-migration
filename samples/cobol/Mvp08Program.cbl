       IDENTIFICATION DIVISION.
       PROGRAM-ID. MVP08PROGRAM.

       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01  WS-LINE              PIC X(40).
       01  WS-AGE               PIC 9(3).
       01  WS-GENDER            PIC X(1).
       01  WS-CLASS             PIC X(10).

       PROCEDURE DIVISION.
           ACCEPT WS-LINE
           MOVE FUNCTION NUMVAL(WS-LINE(1:3)) TO WS-AGE
           MOVE WS-LINE(5:1) TO WS-GENDER

           EVALUATE TRUE ALSO TRUE
               WHEN (WS-AGE >= 20) ALSO (WS-GENDER = "M")
                   MOVE "ADULT-M" TO WS-CLASS
               WHEN (WS-AGE >= 20) ALSO (WS-GENDER = "F")
                   MOVE "ADULT-F" TO WS-CLASS
               WHEN (WS-AGE < 20) ALSO (WS-GENDER = "M")
                   MOVE "MINOR-M" TO WS-CLASS
               WHEN (WS-AGE < 20) ALSO (WS-GENDER = "F")
                   MOVE "MINOR-F" TO WS-CLASS
               WHEN OTHER
                   MOVE "OTHER" TO WS-CLASS
           END-EVALUATE

           DISPLAY "AGE=" WS-AGE "|GENDER=" WS-GENDER "|CLASS=" WS-CLASS

           GOBACK.

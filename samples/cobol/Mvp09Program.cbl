       IDENTIFICATION DIVISION.
       PROGRAM-ID. MVP09PROGRAM.

       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01  WS-FLAG              PIC 9 VALUE 0.
       01  WS-OUT               PIC X(3) VALUE SPACES.
       01  WS-MARK              PIC X(1) VALUE SPACE.

       PROCEDURE DIVISION.
           ACCEPT WS-FLAG

           IF WS-FLAG = 1
               PERFORM PARA-MARK
           END-IF

           PERFORM PARA-A THRU PARA-C-EXIT

           DISPLAY "MARK=" WS-MARK "|SEQ=" WS-OUT

           GOBACK.

       PARA-MARK.
           MOVE "Y" TO WS-MARK.
           EXIT.

       PARA-A.
           MOVE "A" TO WS-OUT(1:1).
           EXIT.

       PARA-B.
           MOVE "B" TO WS-OUT(2:1).
           EXIT.

       PARA-C.
           MOVE "C" TO WS-OUT(3:1).
           EXIT.

       PARA-C-EXIT.
           EXIT.

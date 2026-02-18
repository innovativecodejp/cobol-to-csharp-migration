       IDENTIFICATION DIVISION.
       PROGRAM-ID. MVP14PROGRAM.

       ENVIRONMENT DIVISION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
           SELECT IDX-FILE ASSIGN TO DYNAMIC WS-IDX-PATH
               ORGANIZATION IS INDEXED
               ACCESS MODE  IS DYNAMIC
               RECORD KEY   IS IDX-KEY
               FILE STATUS  IS WS-FS.

       DATA DIVISION.
       FILE SECTION.
       FD  IDX-FILE.
       01  IDX-REC.
           05 IDX-KEY         PIC 9(4).
           05 IDX-TEXT        PIC X(10).

       WORKING-STORAGE SECTION.
       01  WS-IDX-PATH        PIC X(260).
       01  WS-FS              PIC XX.
       01  WS-CASE            PIC 9 VALUE 1.
       01  WS-STARTKEY        PIC 9(4) VALUE 0.
       01  WS-EOF             PIC X VALUE "N".
           88 EOF             VALUE "Y".
           88 NOT-EOF         VALUE "N".

       PROCEDURE DIVISION.
           MOVE "mvp14-index.dat" TO WS-IDX-PATH.
           ACCEPT WS-IDX-PATH FROM ENVIRONMENT "MVP14_INDEX".
           IF WS-IDX-PATH = SPACES
               MOVE "mvp14-index.dat" TO WS-IDX-PATH
           END-IF.

           ACCEPT WS-CASE FROM ENVIRONMENT "MVP14_CASE".
           IF WS-CASE NOT = 2
               MOVE 1 TO WS-CASE
           END-IF.

      * A) Create indexed file in-program.
           OPEN OUTPUT IDX-FILE.
           MOVE 1 TO IDX-KEY.
           MOVE "AAA" TO IDX-TEXT.
           WRITE IDX-REC.
           MOVE 3 TO IDX-KEY.
           MOVE "CCC" TO IDX-TEXT.
           WRITE IDX-REC.
           MOVE 5 TO IDX-KEY.
           MOVE "EEE" TO IDX-TEXT.
           WRITE IDX-REC.
           CLOSE IDX-FILE.

      * B) START KEY >= then READ NEXT.
           OPEN I-O IDX-FILE.
           IF WS-CASE = 1
               MOVE 3 TO WS-STARTKEY
           ELSE
               MOVE 2 TO WS-STARTKEY
           END-IF.

           START IDX-FILE KEY >= WS-STARTKEY
               INVALID KEY
                   SET EOF TO TRUE
           END-START.

           IF NOT EOF
               PERFORM UNTIL EOF
                   READ IDX-FILE NEXT
                       AT END
                           SET EOF TO TRUE
                       NOT AT END
                           DISPLAY "KEY=" IDX-KEY "|TEXT=" IDX-TEXT
                   END-READ
               END-PERFORM
           END-IF.

           CLOSE IDX-FILE.
           DISPLAY "DONE".
           GOBACK.

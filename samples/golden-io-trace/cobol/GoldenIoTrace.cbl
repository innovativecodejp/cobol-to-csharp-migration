       IDENTIFICATION DIVISION.
       PROGRAM-ID. GOLDENIOTRACE.

       ENVIRONMENT DIVISION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
           SELECT IN-FILE ASSIGN TO "input.txt"
               ORGANIZATION IS LINE SEQUENTIAL.
           SELECT OUT-FILE ASSIGN TO "output.txt"
               ORGANIZATION IS LINE SEQUENTIAL.
           SELECT IDX-FILE ASSIGN TO "index.dat"
               ORGANIZATION IS INDEXED
               ACCESS MODE IS DYNAMIC
               RECORD KEY IS IDX-KEY
               FILE STATUS IS WS-IDX-FS.

       DATA DIVISION.
       FILE SECTION.
       FD  IN-FILE.
       01  IN-REC                 PIC X(80).

       FD  OUT-FILE.
       01  OUT-REC                PIC X(80).

       FD  IDX-FILE.
       01  IDX-REC.
           05 IDX-KEY             PIC 9(4).
           05 IDX-TEXT            PIC X(10).

       WORKING-STORAGE SECTION.
       01  WS-RUN                 PIC X(20) VALUE "GOLDEN-0001".
       01  WS-RECNO               PIC 9(4) VALUE 0.
       01  WS-EOF                 PIC X VALUE "N".
           88 EOF                 VALUE "Y".
           88 NOT-EOF             VALUE "N".
       01  WS-IDX-FS              PIC XX.
       01  WS-START-KEY           PIC 9(4) VALUE 3.

       PROCEDURE DIVISION.
           OPEN OUTPUT OUT-FILE.
           OPEN INPUT IN-FILE.

      * START sample trace (minimal positioning event)
           OPEN I-O IDX-FILE.
           START IDX-FILE KEY >= WS-START-KEY
               INVALID KEY
                   DISPLAY "RUN=" WS-RUN
                       "|STMT=S001|TYPE=START|FILE=IDX1|KEY=0003|RESULT=INVALID"
               NOT INVALID KEY
                   DISPLAY "RUN=" WS-RUN
                       "|STMT=S001|TYPE=START|FILE=IDX1|KEY=0003|RESULT=OK"
           END-START.
           CLOSE IDX-FILE.

           PERFORM UNTIL EOF
               READ IN-FILE
                   AT END
                       SET EOF TO TRUE
                       DISPLAY "RUN=" WS-RUN
                           "|STMT=R999|TYPE=READ|FILE=IN1|RESULT=EOF"
                   NOT AT END
                       ADD 1 TO WS-RECNO
                       DISPLAY "RUN=" WS-RUN
                           "|STMT=R001|TYPE=READ|FILE=IN1|RESULT=OK|RECNO=" WS-RECNO

                       IF IN-REC(1:1) = "A"
                           DISPLAY "RUN=" WS-RUN
                               "|STMT=I001|TYPE=IF|COND=FIRST==A|RESULT=TRUE"
                       ELSE
                           DISPLAY "RUN=" WS-RUN
                               "|STMT=I001|TYPE=IF|COND=FIRST==A|RESULT=FALSE"
                       END-IF

                       MOVE IN-REC TO OUT-REC
                       DISPLAY "RUN=" WS-RUN
                           "|STMT=M001|TYPE=ASSIGN|VAR=OUT-REC|VAL=" OUT-REC

                       WRITE OUT-REC
                       DISPLAY "RUN=" WS-RUN
                           "|STMT=W001|TYPE=WRITE|FILE=OUT1|RECNO=" WS-RECNO
               END-READ
           END-PERFORM.

           CLOSE IN-FILE.
           CLOSE OUT-FILE.

           DISPLAY "RUN=" WS-RUN "|STMT=D001|TYPE=DISPLAY|TEXT=DONE".
           GOBACK.

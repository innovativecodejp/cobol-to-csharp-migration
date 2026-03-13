       IDENTIFICATION DIVISION.
       PROGRAM-ID. MVP14INTEGRATION.

      * MVP14 Integration: READ -> WRITE passthrough
      * Minimal COBOL sample for end-to-end I/O verification.
       ENVIRONMENT DIVISION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
           SELECT IN-FILE  ASSIGN TO DYNAMIC WS-IN-PATH
               ORGANIZATION IS LINE SEQUENTIAL.
           SELECT OUT-FILE ASSIGN TO DYNAMIC WS-OUT-PATH
               ORGANIZATION IS LINE SEQUENTIAL.

       DATA DIVISION.
       FILE SECTION.
       FD  IN-FILE.
       01  IN-REC                 PIC X(80).

       FD  OUT-FILE.
       01  OUT-REC                PIC X(80).

       WORKING-STORAGE SECTION.
       01  WS-IN-PATH             PIC X(260).
       01  WS-OUT-PATH            PIC X(260).
       01  WS-EOF                 PIC X VALUE "N".
           88 EOF                 VALUE "Y".
           88 NOT-EOF             VALUE "N".

       PROCEDURE DIVISION.
           MOVE "mvp14-in.txt"  TO WS-IN-PATH.
           MOVE "mvp14-out.txt" TO WS-OUT-PATH.
           ACCEPT WS-IN-PATH  FROM ENVIRONMENT "MVP14_INPUT".
           ACCEPT WS-OUT-PATH FROM ENVIRONMENT "MVP14_OUTPUT".

           OPEN INPUT  IN-FILE.
           OPEN OUTPUT OUT-FILE.

           PERFORM UNTIL EOF
               READ IN-FILE
                   AT END
                       SET EOF TO TRUE
                   NOT AT END
                       MOVE IN-REC TO OUT-REC
                       WRITE OUT-REC
               END-READ
           END-PERFORM.

           CLOSE IN-FILE.
           CLOSE OUT-FILE.
           GOBACK.

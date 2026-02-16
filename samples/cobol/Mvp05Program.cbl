       IDENTIFICATION DIVISION.
       PROGRAM-ID. MVP05PROGRAM.

       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01  WS-TEXT              PIC X(40).

       PROCEDURE DIVISION.
           *> Read one input line into WS-TEXT.
           ACCEPT WS-TEXT

           INSPECT WS-TEXT
               CONVERTING "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                          TO "abcdefghijklmnopqrstuvwxyz"

           DISPLAY "TEXT=" WS-TEXT

           GOBACK.

set INSTNAME=LocalDBnopCommercev1
set DBNAME=Louiemart

call "C:\Program Files\Microsoft SQL Server\120\Tools\Binn\SqlLocalDB.exe" create %INSTNAME%
call "C:\Program Files\Microsoft SQL Server\120\Tools\Binn\SqlLocalDB.exe" start %INSTNAME%
call "C:\Program Files\Microsoft SQL Server\120\Tools\Binn\SqlLocalDB.exe" info %INSTNAME%

echo To Install nopCommerce:
echo Sql Server name: (localdb)\%INSTNAME%
echo Database name:  (localdb)\%DBNAME%
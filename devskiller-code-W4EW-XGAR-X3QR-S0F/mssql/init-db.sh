
cd tmp
sleep 10
echo "DELAY TO GET THE DB READY" 
/opt/mssql-tools/bin/sqlcmd -S blog.db -U sa -P MyPass@word -d master -i 1-SetupStructure.sql 

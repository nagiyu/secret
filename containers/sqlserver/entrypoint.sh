#!/bin/bash

# 一時的にrootユーザーに切り替えてパーミッションを設定
sudo chown -R 10001:0 /var/opt/mssql
sudo chmod -R 755 /var/opt/mssql

# Start SQL Server in the background
/opt/mssql/bin/sqlservr &

# Function to check if SQL Server is ready
function wait_for_sqlserver() {
    echo "Waiting for SQL Server to start..."
    until /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -Q 'SELECT 1' &>/dev/null
    do
        echo -n "."
        sleep 1
    done
    echo "SQL Server is up."
}

# Wait for SQL Server to be ready
wait_for_sqlserver

# Run the SQL scripts in the specified directory
for f in /sql-scripts/*.sql
do
    if [ -f "$f" ]; then
        echo "Running $f"
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -i "$f"
        if [ $? -eq 0 ]; then
            echo "$f executed successfully"
        else
            echo "Error occurred while executing $f"
        fi
    fi
done

# Keep the container running
wait

#!/bin/bash

# 一時的にrootユーザーに切り替えてパーミッションを設定
sudo chown -R 10001:0 /var/opt/mssql
sudo chmod -R 755 /var/opt/mssql

# SQL Server の起動
exec "$@"

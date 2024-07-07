FROM mcr.microsoft.com/mssql/server:2019-latest

# 必要なパッケージをインストール
USER root
RUN apt-get update && apt-get install -y sudo

# mssql ユーザーに sudo 権限を付与
RUN echo 'mssql ALL=(ALL) NOPASSWD:ALL' >> /etc/sudoers

# スクリプトをコンテナ内にコピー
COPY entrypoint.sh /usr/local/bin/entrypoint.sh

# スクリプトに実行権限を付与
RUN chmod +x /usr/local/bin/entrypoint.sh

# mssqlユーザーに戻す
USER mssql

# エントリーポイントスクリプトを実行
ENTRYPOINT ["/usr/local/bin/entrypoint.sh"]

# デフォルトのコマンド
CMD ["/opt/mssql/bin/sqlservr"]
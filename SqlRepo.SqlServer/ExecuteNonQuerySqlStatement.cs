﻿using System;
using System.Threading.Tasks;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class ExecuteNonQuerySqlStatement : ExecuteSqlStatementBase<int>
    {
        public ExecuteNonQuerySqlStatement(IStatementExecutor statementExecutor)
            : base(statementExecutor) { }

        public override int Go()
        {
            if(string.IsNullOrWhiteSpace(this.Sql))
            {
                throw new MissingSqlException();
            }

            return this.StatementExecutor.ExecuteNonQuery(this.Sql);
        }

        public override async Task<int> GoAsync()
        {
            if(string.IsNullOrWhiteSpace(this.Sql))
            {
                throw new MissingSqlException();
            }

            return await this.StatementExecutor.ExecuteNonQueryAsync(this.Sql);
        }
    }
}
﻿/********************************************************************************
Copyright (C) Binod Nepal, Mix Open Foundation (http://mixof.org).

This file is part of MixERP.

MixERP is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MixERP is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MixERP.  If not, see <http://www.gnu.org/licenses/>.
***********************************************************************************/
using System.Collections.ObjectModel;
using System.Data;
using MixERP.Net.Utilities.PgDoc.DBFactory;
using MixERP.Net.Utilities.PgDoc.Helpers;
using MixERP.Net.Utilities.PgDoc.Models;
using Npgsql;

namespace MixERP.Net.Utilities.PgDoc.Processors
{
    internal static class MaterializedViewProcessor
    {
		internal static Collection<PgMaterializedView> GetMaterializedViews(string schemaPattern = ".*", string xSchemaPattern = "")
        {
            Collection<PgMaterializedView> materializedViews = new Collection<PgMaterializedView>();

            string sql = FileHelper.ReadSqlResource("materialized-views-by-schema.sql");

            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
				command.Parameters.AddWithValue("@SchemaPattern", schemaPattern);
				command.Parameters.AddWithValue("@xSchemaPattern", xSchemaPattern);
				using (DataTable table = DbOperation.GetDataTable(command))
                {
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            PgMaterializedView materializedView = new PgMaterializedView
                            {
                                RowNumber = Conversion.TryCastLong(row["row_number"]),
                                Name = Conversion.TryCastString(row["object_name"]),
                                SchemaName = Conversion.TryCastString(row["object_schema"]),
                                Tablespace = Conversion.TryCastString(row["tablespace"]),
                                Owner = Conversion.TryCastString(row["owner"]),
                                Definition = Conversion.TryCastString(row["definition"]),
                                Description = Conversion.TryCastString(row["description"])
                            };


                            materializedViews.Add(materializedView);
                        }
                    }
                }
            }

            return materializedViews;
        }
    }
}
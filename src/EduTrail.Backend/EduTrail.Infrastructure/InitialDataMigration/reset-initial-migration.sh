rm ./src/EduTrail.Backend/EduTrail.Infrastructure/Migrations/*;

dotnet ef migrations add initial_migration \
-p src/EduTrail.Backend/EduTrail.Infrastructure \
-s src/EduTrail.Backend/EduTrail.API;

dotnet ef migrations add initial_data_setup \
-p src/EduTrail.Backend/EduTrail.Infrastructure \
-s src/EduTrail.Backend/EduTrail.API;

cat <<EOF >"$(ls -d ./src/EduTrail.Backend/EduTrail.Infrastructure/Migrations/* | grep "initial_data_setup.cs")"
using EduTrail.Infrastructure.Migrations.DataMigrations;

#nullable disable

namespace EduTrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial_data_setup : InitialDataMigration
    {
    }
}
EOF

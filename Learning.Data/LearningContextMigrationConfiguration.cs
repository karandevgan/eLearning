using System.Data.Entity.Migrations;

namespace Learning.Data {
    public class LearningContextMigrationConfiguration : DbMigrationsConfiguration<LearningContext> {
        public LearningContextMigrationConfiguration() {
            this.AutomaticMigrationDataLossAllowed = true;
            this.AutomaticMigrationsEnabled = true;
        }


#if DEBUG
        protected override void Seed(LearningContext context) {
            new LearningDataSeeder(context).Seed();
        }
#endif
    }
}
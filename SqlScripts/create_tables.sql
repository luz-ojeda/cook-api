CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Ingredients" (
    "Id" uuid NOT NULL,
    "Name" character varying(50) NOT NULL,
    "Substitutions" varchar(50)[] NOT NULL,
    CONSTRAINT "PK_Ingredients" PRIMARY KEY ("Id")
);

CREATE TABLE "Recipes" (
    "Id" uuid NOT NULL,
    "Name" character varying(150) NOT NULL,
    "Ingredients" varchar(200)[] NOT NULL,
    "Instructions" text NOT NULL,
    "Pictures" varchar(200)[] NOT NULL DEFAULT ARRAY[]::varchar(200)[],
    "Videos" varchar(200)[] NOT NULL DEFAULT ARRAY[]::varchar(200)[],
    "PreparationTime" integer NULL,
    "CookingTime" integer NULL,
    "Servings" integer NULL,
    "Difficulty" character varying(6) NULL,
    "Vegetarian" boolean NOT NULL DEFAULT FALSE,
    CONSTRAINT "PK_Recipes" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_Recipe_CookingTime" CHECK ("CookingTime" >= 0 OR "PreparationTime" IS NULL),
    CONSTRAINT "CK_Recipe_Difficulty" CHECK ("Difficulty" IN ('Easy', 'Medium', 'Hard') OR "Difficulty" IS NULL),
    CONSTRAINT "CK_Recipe_Ingredients" CHECK (cardinality("Ingredients") > 0),
    CONSTRAINT "CK_Recipe_PreparationTime" CHECK ("PreparationTime" > 0 OR "PreparationTime" IS NULL)
);

CREATE UNIQUE INDEX "IX_Ingredients_Name" ON "Ingredients" ("Name");

CREATE INDEX "IX_Recipes_Ingredients" ON "Recipes" USING gin ("Ingredients");

CREATE UNIQUE INDEX "IX_Recipes_Name" ON "Recipes" ("Name");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240123225333_InitialMigrate', '7.0.11');

COMMIT;

START TRANSACTION;

ALTER TABLE "Recipes" ALTER COLUMN "Videos" SET DEFAULT ARRAY[]::varchar(200)[];

ALTER TABLE "Recipes" ALTER COLUMN "Pictures" SET DEFAULT ARRAY[]::varchar(200)[];

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240130185601_InitialCreate', '7.0.11');

COMMIT;
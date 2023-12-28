CREATE TABLE Recipes (
    Id UUID PRIMARY KEY,
    Name VARCHAR(150) UNIQUE,
    Ingredients VARCHAR(50)[] NOT NULL CHECK (cardinality(Ingredients) > 0),
    Instructions TEXT NOT NULL,
	Pictures VARCHAR(200)[] NOT NULL DEFAULT '{}',
    Videos VARCHAR(200)[] NOT NULL DEFAULT '{}',
    PreparationTime INTEGER NOT NULL CHECK (PreparationTime >= 0), -- minutes
    CookingTime INTEGER NOT NULL CHECK (CookingTime >= 0), -- minutes
    Servings INTEGER NOT NULL CHECK (Servings > 0),
    Difficulty recipe_difficulty NOT NULL,
    Tried BOOLEAN DEFAULT FALSE,
	Vegetarian BOOLEAN DEFAULT FALSE,
    CONSTRAINT chk_difficulty_valid CHECK (Difficulty IS NULL OR Difficulty IN ('easy', 'medium', 'hard'))
);

-- Index on Name column
CREATE INDEX idx_recipe_name ON Recipes (Name);

-- Index on Ingredients column
CREATE INDEX idx_recipe_ingredients ON Recipes USING GIN(Ingredients);

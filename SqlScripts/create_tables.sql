CREATE TABLE public."Ingredients" (
    "Id" uuid NOT NULL,
    "Name" character varying(50) NOT NULL,
    "Substitutions" character varying(50)[] NOT NULL
);

ALTER TABLE public."Ingredients" OWNER TO postgres;

CREATE TABLE public."Recipes" (
    "Id" uuid NOT NULL,
    "Name" character varying(150) NOT NULL,
    "Ingredients" character varying(200)[] NOT NULL,
    "Instructions" text NOT NULL,
    "Pictures" character varying(200)[] DEFAULT (ARRAY[]::character varying[])::character varying(200)[] NOT NULL,
    "Videos" character varying(200)[] DEFAULT (ARRAY[]::character varying[])::character varying(200)[] NOT NULL,
    "PreparationTime" integer,
    "CookingTime" integer,
    "Servings" integer,
    "Difficulty" character varying(6),
    "Vegetarian" boolean DEFAULT false NOT NULL,
    "Summary" character varying(150),
    CONSTRAINT "CK_Recipe_CookingTime" CHECK ((("CookingTime" >= 0) OR ("PreparationTime" IS NULL))),
    CONSTRAINT "CK_Recipe_Difficulty" CHECK (((("Difficulty")::text = ANY ((ARRAY['Easy'::character varying, 'Medium'::character varying, 'Hard'::character varying])::text[])) OR ("Difficulty" IS NULL))),
    CONSTRAINT "CK_Recipe_Ingredients" CHECK ((cardinality("Ingredients") > 0)),
    CONSTRAINT "CK_Recipe_PreparationTime" CHECK ((("PreparationTime" > 0) OR ("PreparationTime" IS NULL)))
);


ALTER TABLE public."Recipes" OWNER TO postgres;

--
-- TOC entry 3186 (class 2606 OID 17561)
-- Name: Ingredients PK_Ingredients; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Ingredients"
    ADD CONSTRAINT "PK_Ingredients" PRIMARY KEY ("Id");


--
-- TOC entry 3190 (class 2606 OID 17575)
-- Name: Recipes PK_Recipes; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Recipes"
    ADD CONSTRAINT "PK_Recipes" PRIMARY KEY ("Id");


--
-- TOC entry 3184 (class 1259 OID 17576)
-- Name: IX_Ingredients_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Ingredients_Name" ON public."Ingredients" USING btree ("Name");


--
-- TOC entry 3187 (class 1259 OID 17577)
-- Name: IX_Recipes_Ingredients; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Recipes_Ingredients" ON public."Recipes" USING gin ("Ingredients");


--
-- TOC entry 3188 (class 1259 OID 17578)
-- Name: IX_Recipes_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Recipes_Name" ON public."Recipes" USING btree ("Name");
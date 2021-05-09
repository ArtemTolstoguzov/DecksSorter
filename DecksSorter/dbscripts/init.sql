\connect Decks

CREATE TABLE decks(
  deck_id SERIAL PRIMARY KEY, 
  name VARCHAR(36) UNIQUE NOT NULL
);

CREATE TYPE card_values AS ENUM (
  'Ace', 'Two', 'Three', 'Four', 'Five', 
  'Six', 'Seven', 'Eight', 'Nine', 'Ten', 
  'Jack', 'Queen', 'King'
);

CREATE TYPE card_suits AS ENUM (
  'Clubs', 'Diamonds', 'Hearts', 'Spades'
);

CREATE TABLE cards(
  card_id SERIAL PRIMARY KEY, 
  value card_values NOT NULL, 
  suit card_suits NOT NULL, 
  deck_id INT REFERENCES decks ON DELETE CASCADE ON UPDATE RESTRICT NOT NULL, 
  position_in_deck INT CHECK (
    position_in_deck >= 0 
    AND position_in_deck <= 51
  ) NOT NULL, 
  UNIQUE (deck_id, value, suit)
);

CREATE OR REPLACE FUNCTION adding_cards_in_deck()
RETURNS TRIGGER AS
$BODY$
DECLARE
  pos INT := 0;
  suit TEXT;
  value TEXT;
BEGIN
  FOReach value IN ARRAY (SELECT ENUM_RANGE(NULL :: card_values))
  LOOP
    FOReach suit IN ARRAY (SELECT ENUM_RANGE(NULL :: card_suits))
    LOOP 
      INSERT INTO cards(value, suit, deck_id, position_in_deck) 
      VALUES (value :: card_values, suit :: card_suits, NEW.deck_id, pos);
      pos := pos + 1;
    END LOOP;
  END LOOP;
  RETURN NEW;
END
$BODY$
LANGUAGE plpgsql;

CREATE TRIGGER deck_adding 
AFTER INSERT ON decks
FOR EACH ROW
EXECUTE PROCEDURE adding_cards_in_deck();

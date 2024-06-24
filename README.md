# Raccoon-s-Datainterchange-Object\
My own data interchange format created to fit the design of my projects.\
# RDO's grammar:\
**`Symbols written in bold text are terminals.`**\
Object List &#8594; Object+\
Object &#8594; **{** Pair+ **}**\
Pair &#8594; Value **:** Array\
Value &#8594; (**AlphanumericCharacter**)+ | String\
Array &#8594; **[** Value (**WhiteSpace** Value)+ **]**\
String &#8594; **"**  (**AlphanumricCharacter** |**Whitespace**)* **"**\




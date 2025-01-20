enum CardValue {
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Bube = 11,
    Queen = 12,
    King = 13,
    Ace = 1,
}

enum CardSign
{
    Herz = 1,
    Shippe = 2,
    Karo = 3,
    Kreuz = 4
}

interface Card {
    readonly sign: CardSign
    readonly value: CardValue
}

export {
    type Card,
    CardSign,
    CardValue
}
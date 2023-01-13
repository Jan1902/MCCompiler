namespace CompilerTest.Compiling.Transformation.Enums;

//public enum Operations
//{
//    NoOperation,
//    Add,
//    Subtract,
//    ShiftRight,
//    ShiftLeft,
//    Increment,
//    Decrement,
//    PortStore,
//    PortLoad,
//    MemoryStore,
//    MemoryLoad,
//    LoadImmediate,
//    AddImmediate,
//    Branch,
//    Label,
//    Halt
//}

public enum Operations
{
    NOP, //No Operation
    ADD, //Add
    SUB, //Subtract
    RSH, //Right Shift
    LSH, //Left Shift
    BSR, //Barrel Shift Right
    BSL, //Barrel Shift Left
    MLT, //Multiply
    DIV, //Divide
    MOD, //Modulo
    INC, //Increment
    DEC, //Decrement
    IN,  //Port Load
    OUT, //Port Store
    STR, //Memory Store
    LOD, //Memory Load
    IMM, //Immediate
    ADI, //Add Immediate
    JMP, //Jump
    BRG, //Branch if greater
    BGE, //Branch if greater or equal
    BRZ, //Branch if zero
    BNZ, //Branch if not zero
    BRC, //Branch on carry
    BNC, //Branch on no carry
    BRE, //Branch if equal
    BNE, //Branch if not equal
    BRL, //Branch if less
    BLE, //Branch if less or equal
    BRP, //Branch if positive
    BRN, //Branch if negative
    LBL, //Label, maybe remove this later
    HLT, //Halt
    AND, //AND
    NAND,//NAND
    OR,  //OR
    NOR, //NOR,
    NOT, //NOT
    XOR, //XOR
    XNOR,//XNOR
    NEG, //Negate
    PSH, //Push to Stack
    POP, //Pop off of Stack
    RET, //Return
    MOV, //Move
    CAL  //Call
}

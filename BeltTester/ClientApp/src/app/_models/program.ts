import { Stance } from "./stance";
import { Move } from "./move";
import { Technique } from "./technique";

export interface Program {
  id: number,
  graduation: number,
  graduationtype: string,
  name: string,
  stylename: string,
  combinations: Combination[]
}

export interface Combination {
  id: number,
  programid: number,
  sequencenumber: number,
  motions: Motion[]
}

export interface Motion {
  id: number,
  sequencenumber: number,
  stance: Stance,
  move: Move,
  technique: Technique,
  annotation: string
}

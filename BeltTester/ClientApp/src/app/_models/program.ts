import { Stance } from "./stance";
import { Move } from "./move";
import { Technique } from "./technique";

export interface Program {
  id: number,
  graduation: number,
  graduationType: string,
  graduationColor: string,
  name: string,
  styleName: string,
  kihonCombinations: Combination[]
}

export interface Combination {
  id: number,
  programId: number,
  sequenceNumber: number,
  motions: Motion[]
}

export interface Motion {
  id: number,
  sequenceNumber: number,
  stance: Stance,
  move: Move,
  technique: Technique,
  annotation: string
}

INCLUDE ../Templates/SceneInitialization.ink
INCLUDE ../Options.ink
INCLUDE ../Templates/Macros.ink
INCLUDE ../Templates/FailureStates.ink
INCLUDE StartingEvidence.ink

<- Part5StartingEvidence
<- COURT_TMPH
<- Failures.TMPH


&JUMP_TO_POSITION:Witness
&FADE_IN:2
&PLAY_SONG:fyiIWannaXYourExaminationModerato,{songFadeTime}
<- CrossExamination

&SPEAK:Ross

-> Line1

=== Line1 ===
&SCENE:TMPHCourt
&JUMP_TO_POSITION:Witness
&SPEAK:Ross
<color=green>I was animating by myself over in my room at the office.
+ [Continue]
    -> Line2
+ [Press]
    -> Line1Press

=== Line2 ===
&SCENE:TMPHCourt
&JUMP_TO_POSITION:Witness
&SPEAK:Ross
<color=green>But then{ellipsis} I saw someone taking the dinos!!
+ [Continue]
    -> Line3
+ [Press]
    -> Line2Press

=== Line3 ===
&SCENE:TMPHCourt
&JUMP_TO_POSITION:Witness
&SPEAK:Ross
<color=green>It was Jory! He was on the 10 Minute Power Hour set taking the dinos!#correct
+ [Continue]
    -> Line4
+ [Press]
    -> Finale

=== Line4 ===
&SCENE:TMPHCourt
&JUMP_TO_POSITION:Witness
&SPEAK:Ross
<color=green>Now that I know they were stolen, that means the culprit must be Jory!
+ [Continue]
    -> Line1
+ [Press]
    -> Line4Press

=== Line1Press ===
&HOLD_IT:Arin
&PAN_TO_POSITION:Defense,0.5
&WAIT:0.5
&SPEAK:Arin
What were you animating?

&JUMP_TO_POSITION:Prosecution
&SPEAK:TutorialBoy
Your Honor, this is clearly irrelevant to the case.

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
I agree. Arin, try being serious about this.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:Prosecution
&SPEAK:TutorialBoy
Ross, continue your testimony.

-> Line2

=== Line2Press ===
&HOLD_IT:Arin
&PAN_TO_POSITION:Defense,0.5
&WAIT:0.5
&SPEAK:Arin
Who did you see?

&JUMP_TO_POSITION:Witness
&SPEAK:Ross
I'm getting to it, just be patient. I'm trying to build suspense for the viewers!

&JUMP_TO_POSITION:Defense
&SPEAK:Arin
But this isn't being broadcasted{ellipsis}

&JUMP_TO_POSITION:Prosecution
&SPEAK:TutorialBoy
Quick! Back to the testimony before we break the fourth wall again!

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Witness, carry on.
-> Line3

=== Line4Press ===
&HOLD_IT:Arin
&PAN_TO_POSITION:Defense,0.5
&WAIT:0.5
&SPEAK:Arin
What makes you so sure that the dinos were stolen, anyways!?

&JUMP_TO_POSITION:Witness
&SPEAK:Ross
{ellipsis}

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
{ellipsis}

&SCENE:TMPHAssistant
&SPEAK:Dan
Arin, that's literally the reason we're all here.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:Defense
&SPEAK:Arin
{ellipsis}

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
I'll just pretend that didn't happen.
-> Line1

=== Finale ===
&OBJECTION:Arin
&PAN_TO_POSITION:Defense,0.5
&PLAY_EMOTION:Objection
&SPEAK:Arin
You said you saw Jory in the 10 Minute Power Hour room, correct?

&JUMP_TO_POSITION:Witness
&SPEAK:Ross
Yes, that's correct!

&JUMP_TO_POSITION:Defense
&SET_POSE:PaperSlap
&SPEAK:Arin
Yet you also say you were in your office animating
&SET_POSE:Confident
Seems very odd to me! How could you see anyone while you were focused on your work!

&JUMP_TO_POSITION:Prosecution
&SPEAK:TutorialBoy
Are you saying that my witness is a liar?
I'm sure Ross has a very reasonable explanation for all this.

&SCENE:TMPHAssistant
&SPEAK:Dan
He oughta have a real good reason for this.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:Witness
&SPEAK:Ross
T-That's right! The reason I was able to see Jory was{ellipsis} because I needed to poop!
Yeah!

&JUMP_TO_POSITION:Defense
&SET_POSE:Annoyed
&SPEAK:Arin
Um{ellipsis} excuse me?

&SCENE:TMPHAssistant
&SPEAK:Dan
Hah hah hah hah!!!

&SCENE:TMPHCourt
&JUMP_TO_POSITION:Defense
&THINK:Arin
(Goddamnit, Ross.)

&SPEAK:Arin
What does you needing to poop have to do with seeing Jory?

&JUMP_TO_POSITION:Witness
&SPEAK:Ross
W-Well, you see, I had to go out to use the bathroom, which is how I saw Jory!

&JUMP_TO_POSITION:Defense
&SPEAK:Arin
Uh-huh{ellipsis}
&SET_POSE:Normal
Your Honor, I believe this needs to be added to the witness's testimony.

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Agreed. Witness, add your poop story to your testimony.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:Witness
&SPEAK:Ross
Uh{ellipsis} Yes, why of course, Your Honor. Let me go over it again.

&SCENE:TMPHAssistant
&SPEAK:Dan
Way to go, Big Cat! Let's see how this changes things.

&HIDE_TEXTBOX
&FADE_OUT:1
&FADE_OUT_SONG:{songFadeTime}
&WAIT:3

&LOAD_SCRIPT:Case1/1-7-RossCrossExamination2

-> END
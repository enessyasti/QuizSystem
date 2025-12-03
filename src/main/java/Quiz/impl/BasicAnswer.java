package Quiz.impl;

import Quiz.Interface.Answer;

public class BasicAnswer implements Answer {

    private final String content;
    private final boolean correct;

    public BasicAnswer(String content, boolean correct) {
        this.content = content;
        this.correct = correct;
    }

    @Override
    public String getContent() {
        return content;
    }

    @Override
    public boolean isCorrect() {
        return correct;
    }
}

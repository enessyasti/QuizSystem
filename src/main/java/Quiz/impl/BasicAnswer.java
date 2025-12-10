package Quiz.impl;

import Quiz.Interface.Answer;

public class BasicAnswer implements Answer {
    private final String content;   // Cevap metni
    private final boolean correct;  // Doğru mu?

    public BasicAnswer(String content, boolean correct) {
        this.content = content;
        this.correct = correct;
    }

    @Override
    public String getContent() { return content; }    // içeriği döndür

    @Override
    public boolean isCorrect() { return correct; }    // doğru mu diye sor
}
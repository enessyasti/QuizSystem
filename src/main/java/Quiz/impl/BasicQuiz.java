package Quiz.impl;

import Quiz.Interface.Question;
import Quiz.Interface.Quiz;
import java.util.List;

public class BasicQuiz implements Quiz {
    private final String title;               // Quiz başlığı
    private final List<Question> questions;   // Soru listesi

    public BasicQuiz(String title, List<Question> questions) {
        this.title = title;
        this.questions = List.copyOf(questions);  // soruları dondur (değişmesin)
    }

    @Override
    public String getTitle() { return title; }

    @Override
    public List<Question> getQuestions() { return questions; }
}
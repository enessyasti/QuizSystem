package Quiz.impl;

import Quiz.Interface.Question;
import Quiz.Interface.Quiz;
import java.util.List;

public class BasicQuiz implements Quiz {
    private final String title;
    private final List<BasicQuestion> questions;

    public BasicQuiz(String title, List<BasicQuestion> questions) {
        this.title = title;
        this.questions = questions;
    }

    @Override
    public String getTitle() {
        return title;
    }

    @Override
    public List<? extends Question> getQuestions() {
        return questions;
    }
}
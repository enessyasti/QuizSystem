package Quiz.impl;

import Quiz.Interface.Question;
import Quiz.Interface.Quiz;

import java.util.List;

public class BasicQuiz implements Quiz {

    private final String title;
    private final List<Question> questions;

    public BasicQuiz(String title, List<Question> questions) {
        this.title = title;
        this.questions = questions;
    }

    @Override
    public String getTitle() {
        return title;
    }

    @Override
    public List<Question> getQuestions() {
        return questions;
    }
}

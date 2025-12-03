package Quiz.impl;

import Quiz.Interface.Answer;
import Quiz.Interface.Question;
import java.util.List;

public class BasicQuestion implements Question {

    private final String prompt;
    private final List<Answer> answers;

    public BasicQuestion(String prompt, List<Answer> answers) {
        this.prompt = prompt;
        this.answers = answers;
    }

    @Override
    public String getPrompt() {
        return prompt;
    }

    @Override
    public List<Answer> getAnswers() {
        return answers;
    }
}

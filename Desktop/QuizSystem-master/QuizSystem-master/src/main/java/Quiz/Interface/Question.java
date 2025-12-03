package Quiz.Interface;

import java.util.List;

public interface Question {
    String getPrompt();
    List<Answer> getAnswers();
}

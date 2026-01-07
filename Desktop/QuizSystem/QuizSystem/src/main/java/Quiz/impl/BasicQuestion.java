package Quiz.impl;

import Quiz.Interface.Answer;
import Quiz.Interface.Question;
import com.google.gson.annotations.SerializedName;
import java.util.List;

public class BasicQuestion implements Question {
    @SerializedName("text")
    private final String prompt;
    private final List<BasicAnswer> answers;

    public BasicQuestion(String prompt, List<BasicAnswer> answers) {
        this.prompt = prompt;
        this.answers = answers;
    }

    @Override
    public String getPrompt() {
        return prompt;
    }

    @Override
    public List<? extends Answer> getAnswers() {
        return answers;
    }

    // Interface gerekliliği: Doğru cevabın sırasını bul
    @Override
    public int getCorrectAnswerIndex() {
        for (int i = 0; i < answers.size(); i++) {
            if (answers.get(i).isCorrect()) {
                return i;
            }
        }
        return -1; // Doğru cevap yoksa
    }
}
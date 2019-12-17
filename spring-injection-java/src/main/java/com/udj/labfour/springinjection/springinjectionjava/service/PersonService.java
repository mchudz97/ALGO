package com.udj.labfour.springinjection.springinjectionjava.service;

import com.udj.labfour.springinjection.springinjectionjava.domain.Person;
import org.springframework.stereotype.Component;

import java.util.List;

public interface PersonService {

    void addPerson(Person person);

    void deletePerson(Person person);

    List<Person> getPersons();

    Person getPerson();
}
